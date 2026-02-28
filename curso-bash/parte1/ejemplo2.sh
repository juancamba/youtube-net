#!/bin/bash

echo "Hola Linux" $1

# sudo apt  install curl
response=$(curl --write-out "%{http_code}\n" --silent --output /dev/null "$1")

echo peticion $1 response: $response