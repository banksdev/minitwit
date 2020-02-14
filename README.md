# MiniTwit

## Run
To run the app with docker-compose:
```bash
cd netcore
docker-compose build
docker-compose up
```
**Note!** The database has to be updated first. Preferably first run database container, then update database and after that run compose.
```bash
dotnet ef database update --project $(pwd)/netcore/Minitwit.DataAccessLayer/Minitwit.DataAccessLayer.csproj
```

                        / MiniTwit /

           because writing todo lists is not fun


    ~ What is MiniTwit?

      A SQLite and Flask powered twitter clone

    ~ How do I use it?

      1. edit the configuration in the minitwit.py file

      2. fire up a python shell and run this:

         >>> from minitwit import init_db; init_db()

      3. now you can run the minitwit.py file with your
         python interpreter and the application will
         greet you on http://localhost:5000/
	
    ~ Is it tested?

      You betcha.  Run the `minitwit_tests.py` file to
      see the tests pass.


## Run Docker image
``` bash
 #To build the image 
 docker build -t minitwit1 .

 # To run the docker image
 docker run --name "dev" -d -p 8080:5000 minitwit1

  # To connect to the docker image
  docker exec -it dev /bin/bash

  # To stop docker container
  docker container stop dev

  # To remove docker container 
  docker container prune
```
