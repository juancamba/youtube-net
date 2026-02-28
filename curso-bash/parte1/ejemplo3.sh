#!/bin/bash
# Validar que se pase una cadena
if [ -z "$1" ]; then
  echo "Uso: $0 <url>"
  exit 1
fi

response=$(curl --write-out "%{http_code}" --silent --output /dev/null "$1")

if [ "$response" -eq 200 ]; then
  echo "OK - $1 responde correctamente"
else
  echo "ERROR - $1 respondió con código $response"
fi