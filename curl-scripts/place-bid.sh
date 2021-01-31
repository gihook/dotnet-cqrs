#!/usr/bin/env bash

curl -H "Content-Type: application/json" -d '{"id": 1, "priceValue": 100000}' "localhost:5000/command/PlaceBid"
