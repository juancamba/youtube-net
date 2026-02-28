#!/bin/bash

LOG="check.log"

response=$(curl -w "%{http_code}" -s -o /dev/null "$1")

if [ "$response" -eq 200 ]; then
  echo "$(date) OK $1" >> "$LOG"
else
  echo "$(date) ERROR $1 ($response)" >> "$LOG"
fi