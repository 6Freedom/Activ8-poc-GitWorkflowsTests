# Identification system

When the user identify itself on the server the following steps are done:

1. The client send it's ids (username and password) throught the TLS connection.
2. The server look in the database for an user that match with the username/userid.
3. The server hash the password (with the user salt)
4. The server compare the hash with the stored password hash assigned with that username.
5. if the hashes matches the server send to the client a session id or a JWT token.

The salt is a random unique string for every user appened (or prepended) 
to the user password. Thanks to salt two users can share the same password without sharing the same hash.
Salt must be stored in the database.

Pepper is similar to salt but unlike this one, pepper is not stored in the database. That way if the database is compromised the hacker would not have access to the pepper and would not be able to crack the hashes.
The main issues with peppers is their long term maintenance. Changing the pepper in use will invalidate all of the existing passwords stored in the database, which means that it can't easily be changed in the event of the pepper being compromised.

links:  
[Password Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#Use_a_cryptographically_strong_credential-specific_salt)  
[Adding Salt to Hashing: A Better Way to Store Passwords](https://auth0.com/blog/adding-salt-to-hashing-a-better-way-to-store-passwords/)  
[How does the attacker know what algorithm and salt to use in a dictionary attack?](https://security.stackexchange.com/questions/180535/how-does-the-attacker-know-what-algorithm-and-salt-to-use-in-a-dictionary-attack/180536#180536)


## Json Web Token (JWT)

> JSON Web Token is used to carry information related to the identity and characteristics (claims) of a client. This information is signed by the server in order for it to detect whether it was tampered with after sending it to the client. This will prevent an attacker from changing the identity or any characteristics (for example, changing the role from simple user to admin or change the client login).

[JSON Web Token Cheat Sheet for Java](https://cheatsheetseries.owasp.org/cheatsheets/JSON_Web_Token_for_Java_Cheat_Sheet.html)  

The JWT is sent to the server for every request and the server validate the token before processing the request.
In order to be validated a JWT must fullify some requirements (claims), specified during its creattion (ex: name of the issuer, name of the subject, expiration time, etc).

A JWT have the following structure:

1. Header, contain the algorithm used to generate the signature and the token type (JWT).
2. Payload, contain a set of claims (both [standards claims](https://en.wikipedia.org/wiki/JSON_Web_Token#Standard_fields) and customs ones)
3. Signature, Securely validates the token. The signature is calculated by encoding the header and payload using Base64url Encoding and concatenating the two together with a period separator.


links:  
[Wikipedia](https://en.wikipedia.org/wiki/JSON_Web_Token)  
[Commencer avec jwt](https://riptutorial.com/fr/jwt)  
[jwt.io](https://jwt.io/)  
[What really is the difference between session and token based authentication ](https://dev.to/thecodearcher/what-really-is-the-difference-between-session-and-token-based-authentication-2o39)  
[JSON Web Tokens](https://auth0.com/docs/tokens/json-web-tokens)  
[JSON Web Token Claims](https://auth0.com/docs/tokens/json-web-tokens/json-web-token-claims)  
[JSON Web Token Cheat Sheet for Java](https://cheatsheetseries.owasp.org/cheatsheets/JSON_Web_Token_for_Java_Cheat_Sheet.html)  
[A Look at The Draft for JWT Best Current Practices](https://auth0.com/blog/a-look-at-the-latest-draft-for-jwt-bcp/)  
[Use JWT The Right Way!](https://stormpath.com/blog/jwt-the-right-way)  
[JwtSecurityToken Class documentation](https://docs.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt.jwtsecuritytoken?view=azure-dotnet)  
[Preuve d’authentification avec JWT](https://blog.ippon.fr/2017/10/12/preuve-dauthentification-avec-jwt/)  
[JWT: The Complete Guide to JSON Web Tokens](https://blog.angular-university.io/angular-jwt/)