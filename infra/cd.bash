# Перекладываем статику в монтируемое nginx место
rm -rf /usr/share/nginx/html/*
mkdir -p /usr/share/nginx/html
cp -rf ~/build/* /usr/share/nginx/html
# Перекладываем конфигурационный файл для nginx
rm -rf /etc/nginx/*
mkdir -p /etc/nginx
cp -rf ~/prod/nginx.conf /etc/nginx

# Логинимся в докере, чтобы при запуске yml файла
# была возможность скачать наш контейнер с Docker Hub
# DOCKER_LOGIN и DOCKER_PWD/DOCKER_ACCESS_TOKEN необходимо заранее задать в 
# environment сервера
docker login https://index.docker.io/v2 -u $DOCKER_LOGIN -p $DOCKER_PWD 

# Запускаем сервис (перезапускаем в фоновом режиме)
docker-compose pull
docker-compose up --force-recreate -d
docker image prune -f



