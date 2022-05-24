curl -o api-def.json http://localhost:5219/swagger/v1/swagger.json
openapi-generator-cli generate -g javascript -i api-def.json -o src/services/api-gen