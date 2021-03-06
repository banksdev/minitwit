FROM ubuntu:18.04
EXPOSE 5000

RUN apt-get update && apt-get install -y software-properties-common
RUN add-apt-repository ppa:deadsnakes/ppa
RUN apt-get update && apt-get install -y python3 python3-pip
RUN python3 -m pip install pip
RUN apt-get update && apt-get install -y python3-distutils python3-setuptools
RUN echo 'alias python="python3.6"' >> ~/.bashrc

RUN pip3 install flask

COPY . /app
WORKDIR /app

RUN ./control.sh init
ENTRYPOINT [ "python3", "minitwit.py" ]