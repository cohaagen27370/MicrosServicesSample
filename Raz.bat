docker stop -t 30 $(docker ps -aq --filter ancestor=seedmicroservice:1.0) || echo "Arrêt du container"
docker rm  -f $(docker ps -aq --filter ancestor=seedmicroservice:1.0) && docker rmi seedmicroservice:1.0  || echo "Suppression du container et de l'image"

docker stop -t 30 $(docker ps -aq --filter ancestor=cropmicroservice:1.0) || echo "Arrêt du container"
docker rm -f $(docker ps -aq --filter ancestor=cropmicroservice:1.0) && docker rmi cropmicroservice:1.0  || echo "Suppression du container et de l'image"

 || echo "Arrêt du container"
docker rm -f $(docker ps -aq --filter ancestor=gateway:1.0) && docker rmi gateway:1.0  || echo "Suppression du container et de l'image"

docker stop -t 30 $(docker ps -aq --filter ancestor=front:1.0) || echo "Arrêt du container"
docker rm -f $(docker ps -aq --filter ancestor=front:1.0) && docker rmi front:1.0  || echo "Suppression du container et de l'image"