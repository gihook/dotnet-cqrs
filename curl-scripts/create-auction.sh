curl -H "Content-Type: application-json" -d '{"name": "Test", "description": "Test Desc", "initialPrice": 43214}' localhost:5000/command/CreateAuction

curl -H "Content-Type: application-json" -d '{"name": "Another Test", "description": "Test Desc", "initialPrice": 43215, "id": 12}' localhost:5000/command/CreateAuction
