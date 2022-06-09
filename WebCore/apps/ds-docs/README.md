# Dominion Design Style Guide & Examples

## Overview 

An Angular project with a self-contained Go webserver and Docker compose to run a self-contained web project that holds our internal style guides and design examples for development purposes. 

Some references for [Go (Golang)](https://golang.org) web server, Angular & how I got them working in a container: 
- [Go in a Docker container](https://firehydrant.io/blog/develop-a-go-app-with-docker-compose/)
- [go-gin-wrapper](https://github.com/hiromaily/go-gin-wrapper/blob/master/cmd/ginserver/main.go)
  - Didn't use this, but it was good reference on how to achieve our result
- [Containerize Angular](https://www.indellient.com/blog/how-to-dockerize-an-angular-application-with-nginx/)
- [Multi-instance Go web servers](https://github.com/gin-gonic/gin/issues/346)
- [Go microservices w/Gin-Gonic](https://medium.com/@emrahkurman1805/building-golang-microservices-with-gin-gonic-673f4fced794)

Adding this in the gin-gonic framework setup was required to make Chrome correctly download JS files: 
```
mime.AddExtensionType(".js", "application/javascript")
```


## How to build everything

1. Open CLI at `~/webcore/apps/ds-docs` 
2. Run: 
```
~/webcore/apps/ds-docs: npm run ds-docs:deploy
```
This single command will install all NPM dependencies, build the ds-docs project in production mode, builds the necessary docker image for consumption by the composer and then initialize the docker compose and starts the container. 

> :bulb:  It is important to note that this command will only work when the site is being served with an IIS redirect rule. The ds-docs:prod build hardcodes /design into the base-href and the deploy url effecting the URLs that are generated in ui/dist/index.html.

![IIS Configuration](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/3d38fa7f-710f-4697-9863-aea95c0e5388/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAT73L2G45O3KS52Y5%2F20211013%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20211013T165526Z&X-Amz-Expires=86400&X-Amz-Signature=50453fbf22f06f8cc66c2addc53c25d563e6cd30d1ac53a4b3bb498cd5376585&X-Amz-SignedHeaders=host&response-content-disposition=filename%20%3D%22Untitled.png%22)
