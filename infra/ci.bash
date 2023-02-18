cd ..
docker build -t backend-api --build-arg ASPNETCORE_ENVIRONMENT=Secrets . -f TestWebApplication/Dockerfile
rm -rf ~/image.tar
docker save -o ~/image.tar backend-api
ssh admin@109.196.164.182 'rm -rf ~/image.tar'
scp ~/image.tar admin@109.196.164.182:~/ 
ssh admin@109.196.164.182 'docker load -i ~/image.tar'
