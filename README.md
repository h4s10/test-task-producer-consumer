## MYSQL:
docker run --name mysqlDB -p 3306:3306 -v /tmp/docker/mySql:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=123123 -e MYSQL_DATABASE=test -e MYSQL_USER=test -e MYSQL_PASSWORD=123123 -d mysql/mysql-server:latest

## RABBIT:
docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq