#!/bin/bash
#Medir tiempo de respuesta
if [ -z "$1" ]; then
  echo "Uso: $0 <url>"
  exit 1
fi
#here-string
read code time <<< $(curl -w "%{http_code} %{time_total}" -s -o /dev/null "$1")

echo "URL: $1"
echo "Código HTTP: $code"
echo "Tiempo: ${time}s"