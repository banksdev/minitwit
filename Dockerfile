FROM ubuntu:18.04

RUN apt-get update && apt-get install -y software-properties-common
RUN add-apt-repository ppa:deadsnakes/ppa
RUN apt-get update && apt-get install -y python3 python3-pip
RUN python3 -m pip install pip
RUN apt-get update && apt-get install -y python3-distutils python3-setuptools
RUN echo 'alias python="python3.6"' >> ~/.bashrc

COPY . /app
WORKDIR /app
# CMD [ "./control.sh", "init" ] 
# CMD "ls"
# CMD [ "python", "--version" ]
# CMD [ "ls" ]
# CMD "/bin/sh control.sh init" 
# CMD "python control.sh init"
#"python -c 'from minitwit import init_db; init_db()'", 