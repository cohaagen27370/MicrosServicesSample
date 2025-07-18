docker network create my-app-network

docker stop -t 30 $(docker ps -aq --filter ancestor=dogmicroservice:1.0)
docker rm -f $(docker ps -aq --filter ancestor=dogmicroservice:1.0)
docker rmi dogmicroservice:1.0
docker build -f Dockerfile-dog -t dogmicroservice:1.0 .
docker run -d -p 5075:5075 --name dogMicro --network my-app-network dogmicroservice:1.0

docker stop -t 30 $(docker ps -aq --filter ancestor=ordermicroservice:1.0)
docker rm -f $(docker ps -aq --filter ancestor=ordermicroservice:1.0)
docker rmi ordermicroservice:1.0
docker build -f Dockerfile-order -t ordermicroservice:1.0 .
docker run -d -p 5104:5104 --name orderMicro --network my-app-network ordermicroservice:1.0

docker stop -t 30 $(docker ps -aq --filter ancestor=gateway:1.0)
docker rm -f $(docker ps -aq --filter ancestor=gateway:1.0)
docker rmi gateway:1.0
docker build -f Dockerfile-gateway -t gateway:1.0 .
docker run -d -p 7181:7181 --name gateway --network my-app-network gateway:1.0

docker stop -t 30 $(docker ps -aq --filter ancestor=front:1.0)
docker rm -f $(docker ps -aq --filter ancestor=front:1.0)
docker rmi front:1.0
docker build -f Dockerfile-front -t front:1.0 .
docker run -d -p 80:80 --name front --network my-app-network front:1.0

docker pull rabbitmq:management
docker run -d --hostname my-rabbit --name some-rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
docker exec -it some-rabbitmq bash
rabbitmqadmin declare queue name=reinitData durable=true
