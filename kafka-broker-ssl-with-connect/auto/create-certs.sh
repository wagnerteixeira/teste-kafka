#!/bin/bash
set -euf -o pipefail

cd "$(dirname "$0")/../secrets/" || exit

echo "🔖  Generating some fake certificates and other secrets."
echo "⚠️  Remember to type in \"yes\" for all prompts."
sleep 2

TLD="local"
PASSWORD="awesomekafka"

# Generate CA key
openssl req -new -x509 -keyout fake-ca-1.key \
	-out fake-ca-1.crt -days 9999 \
	-subj "/CN=ca1.${TLD}/OU=CIA/O=REA/L=Melbourne/S=VIC/C=AU" \
	-passin pass:$PASSWORD -passout pass:$PASSWORD

for i in broker control-center metrics schema-registry rest-proxy connect client; do
	echo "🧳 Creating ${i} certificates"
	# Create keystores
	keytool -genkey -noprompt \
		-alias ${i} \
		-dname "CN=${i}.${TLD}, OU=CIA, O=REA, L=Melbourne, S=VIC, C=AU" \
		-keystore kafka.${i}.keystore.jks \
		-keyalg RSA \
		-storepass $PASSWORD \
		-keypass $PASSWORD \
		-file $i.key

	# Create CSR, sign the key and import back into keystore
	keytool -keystore kafka.$i.keystore.jks -alias $i -certreq -file $i.csr -storepass $PASSWORD -keypass $PASSWORD

	# export private key to pem format
	keytool -importkeystore -srckeystore kafka.$i.keystore.jks -srcstorepass $PASSWORD \
		-srckeypass $PASSWORD  -srcalias ${i}  -destalias ${i}.key \
		-destkeystore kafka.$i.keystore.p12 -deststoretype PKCS12 \
		-deststorepass $PASSWORD -destkeypass $PASSWORD
		
	openssl pkcs12 -in kafka.$i.keystore.p12 -nodes -nocerts -out $i.key.pem -passin pass:$PASSWORD -passout pass:$PASSWORD


	openssl x509 -req -CA fake-ca-1.crt -CAkey fake-ca-1.key -in $i.csr -out $i-ca1-signed.crt -days 9999 -CAcreateserial -passin pass:$PASSWORD

	keytool -keystore kafka.$i.keystore.jks -alias CARoot -import -file fake-ca-1.crt -storepass $PASSWORD -keypass $PASSWORD -noprompt

	keytool -keystore kafka.$i.keystore.jks -alias $i -import -file $i-ca1-signed.crt -storepass $PASSWORD -keypass $PASSWORD

	# Create truststore and import the CA cert.
	keytool -keystore kafka.$i.truststore.jks -alias CARoot -import -file fake-ca-1.crt -storepass $PASSWORD -keypass $PASSWORD -noprompt

	echo $PASSWORD >${i}_sslkey_creds
	echo $PASSWORD >${i}_keystore_creds
	echo $PASSWORD >${i}_truststore_creds
done

echo "✅  All done."
