#!/bin/bash
# Comprobar varias URLs

# ejecutar ./ejemplo5.sh https://google.com https://github.com https://fake.url


for url in "$@"; do
  response=$(curl -w "%{http_code}" -s -o /dev/null "$url")

  if [ "$response" -eq 200 ]; then
    echo "✔ $url OK"
  else
    echo "✘ $url ERROR ($response)"
  fi
done