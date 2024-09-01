# Digital Asset Management

```
{
  "hashing": {
    "saltByteSize": 10,
    "hashByteSize": 10,
    "iteration": 10
  },
  "jwt": {
    "key": "This is the key used to sign and verify json web token, the key size must be greater than 512 bits"
  }
}
```

POST /v1/api/users/registration
POST /v1/api/auth/authentication

GET /v1/api/folders
GET /v1/api/folders/{id}
POST /v1/api/folders
PATCH /v1/api/folders/{id}/movement
PATCH /v1/api/folders/{id}
PATCH /v1/api/folders/{id}/trash
DELETE /v1/api/folders/{id}

POST /v1/api/folders/{id}/permissions
GET /v1/api/folders/{id}/permissions
DELETE /v1/api/folders/{id}/permissions
POST /v1/api/files/{id}/permissions
GET /v1/api/files/{id}/permissions
DELETE /v1/api/files/{id}/permissions