docker network create my-app-network  || echo "Création du sous réseau"

docker build -f Dockerfile-seed -t seedmicroservice:1.0 . || echo "Création de l'image de seedmicroservice"
docker run -d -p 5075:5075 --name seedMicro --network my-app-network seedmicroservice:1.0 || echo "Création du container de seedmicroservice et démarrage"

docker build -f Dockerfile-crop -t cropmicroservice:1.0 . || echo "Création de l'image de cropmicroservice"
docker run -d -p 5104:5104 --name cropMicro --network my-app-network cropmicroservice:1.0 || echo "Création du container de cropmicroservice et démarrage"

docker build -f Dockerfile-gateway -t gateway:1.0 . || echo "Création de l'image de gateway"
docker run -d -p 5023:5023 --name gateway --network my-app-network gateway:1.0 || echo "Création du container de gateway et démarrage"

docker build -f Dockerfile-front -t front:1.0 . || echo "Création de l'image de front"
docker run -d -p 80:80 --name front --network my-app-network front:1.0 || echo "Création du container de front et démarrage"

docker pull rabbitmq:management || echo "Création de l'image de rabbitMQ"
docker run -d --hostname my-rabbit --name some-rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management && docker exec -it some-rabbitmq bash  || echo "Création du container de front et démarrage"
rabbitmqadmin declare queue name=reinitData durable=true  || echo "Création du topic sur rabbitMq"
