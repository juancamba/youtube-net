
# ------------------
# Grep
# ------------------

# n: numero  de linea, #i: sin case sensitive, r: recursivo

grep -nir 404 linux-2/

grep " 404 " access.log*

grep "10/May/2025" access.log*

#invertir la busqueda
grep -v "GET /index.html" access.log

# ------------------
#Sed
# ------------------

#Quitar todas las líneas que sean peticiones a /favicon.ico:
sed '/favicon.ico/d' access.log.2

# Reemplazar todas las direcciones IP 127.0.0.1 por localhost
# sed 's/patrón/reemplazo/modificadores'

sed 's/127\.0\.0\.1/localhost/g' access.log

# Múltiples sustituciones encadenadas con -e
# Primera sustitución: reemplaza 127.0.0.1 por localhost
# Segunda sustitución: reemplaza 202 por OK
sed -e 's/127\.0\.0\.1/localhost/g' -e 's/HTTP\/1\.1" 200/HTTP\/1\.1" OK/g' access.log


#Combinacion de Grep y Sed
grep -n "GET" access.log.* | sed 's/127\.0\.0\.1/localhost/g'

grep -n "GET" access.log.* | sed 's/127\.0\.0\.1/localhost/g' >> resultado.txt

# ------------------
# awk
# ------------------

cat linux-2/access.log
# vemos que tiene el formato 
# IP - - [fecha] "método URL protocolo" código tamaño
awk '{print $1, $9}' access.log*

# Contar cuántos códigos 200 hay
awk '$9 == 200 {count++} END {print "Códigos 200:", count}' access.log*



# Sumar total de bytes transferidos
awk '{bytes += $10} END {print "Total bytes:", bytes}' access.log*


# Ver cuántas veces se usó cada método HTTP (GET, POST, etc.)

awk '{print $6}' access.log* | sed 's/"//g' | sort | uniq -c

# Ver las URLs solicitadas (campo $7)
awk '{print $7}' access.log*


#Ejemplo de línea en error.log
# [Sat May 10 10:16:10.654321 2025] [authz_core:error] [pid 1235] [client 127.0.0.1:54322] AH01630: client denied by server configuration

# Ver errores por día
awk '{print $2, $3}' error.log* | sort | uniq -c

# Mostrar solo las líneas con errores graves (que contienen error pero no warn
awk '/error/ && !/warn/' error.log*


# combinacion de grep, sed y awk
grep -n "GET" access.log.* | sed 's/127\.0\.0\.1/localhost/g' | awk '$9 == 200 {count++} END {print "Códigos 200:", count}' 

