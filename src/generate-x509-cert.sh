KEY_NAME="private"
CRT_NAME="cert"
CSR_NAME="signing"
PFX_NAME="cert"

COUNTRY="VN"
STATE_OR_PROVINCE="Ho Chi Minh"
LOCALITY="Ho Chi Minh"
ORGANIZATION="Sisa Co., Ltd"
ORGANIZATION_UNIT="IT Department"
COMMON_NAME="sisa.io"
EMAIL_ADDRESS="dev@sisa.io"

openssl req \
-x509 \
-newkey rsa:4096 \
-sha512 \
-days 3650 \
-nodes \
-keyout $KEY_NAME.key \
-out $CRT_NAME.crt \
-subj "/C=${COUNTRY}/ST=${STATE_OR_PROVINCE}/L=${LOCALITY}/O=${ORGANIZATION}/OU=${ORGANIZATION_UNIT}/CN=${COMMON_NAME}/emailAddress=${EMAIL_ADDRESS}" \
-extensions v3_ca \
-extensions v3_req \
-config <( \
  echo '[ req ]'; \
  echo 'default_bits                = 4096'; \
  echo 'distinguished_name          = req'; \
  echo 'x509_extension              = v3_ca'; \
  echo 'req_extensions              = v3_req'; \
  echo '[ v3_req ]'; \
  echo 'basicConstraints            = CA:FALSE'; \
  echo 'keyUsage                    = nonRepudiation, digitalSignature, keyEncipherment'; \
  echo 'subjectAltName              = @alt_names'; \
  echo '[ alt_names ]'; \
  echo "DNS.1                       = ${COMMON_NAME}"; \
  echo "DNS.2                       = www.${COMMON_NAME}"; \
  echo '[ v3_ca ]'; \
  echo 'subjectKeyIdentifier        = hash'; \
  echo 'authorityKeyIdentifier      = keyid:always,issuer'; \
  echo 'basicConstraints            = critical, CA:TRUE, pathlen:0'; \
  echo 'keyUsage                    = critical, cRLSign, keyCertSign'; \
  echo 'extendedKeyUsage            = serverAuth, clientAuth')

# openssl x509 -in $CRT_NAME.crt -out $CSR_NAME.csr

# To get a .pfx, use the following command:
# PASSWORD=$(< /dev/urandom | tr -dc a-zA-Z0-9~@$%^_=+ | head -c 20)
# openssl pkcs12 -export -out $PFX_NAME.pfx -inkey $KEY_NAME.key -in $CRT_NAME.crt -passout pass:$PASSWORD

# echo "\nPassword: ${PASSWORD}"
