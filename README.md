## Foreman for windows

Manage Procfile-based applications

[Foreman](https://github.com/ddollar/foreman) in CSharp.

### Installation

- Todo

##### Compile from Source

```sh
  $ build.bat
```

### Usage

```sh
  $ cat Procfile
  web: bin/web start -p $PORT -e $APP_ENV
  worker: bin/worker queue -e $APP_ENV
```

```sh
  $ cat .env
  PORT=6000
  APP_ENV=prod
  MAILER_DSN=smtp://localhost
  DATABASE_DSN=sqlite:///./var/data.db
  TRANSPORT_DSN=amqp://user:password@localhost:5672/%2f/messages
```

```sh
  $ foreman.exe
```
