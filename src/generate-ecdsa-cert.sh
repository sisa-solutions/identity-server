openssl ecparam -name secp521r1 -genkey -noout -out private.key
openssl ec -in private.key -pubout -out public.key