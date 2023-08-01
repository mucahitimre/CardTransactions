# CardTransactions

Simple payment card steps.

## Requirements

- Docker: [Visit Docker website for installation](https://www.docker.com/)

## Installation

1. Pull mongo latest version:
```bash
docker pull mongo:latest
```

2. Run mongo:
```bash
docker run -d -p 27017:27017 --name my-mongodb mongo
```

3. If there is mongo with the same name:
```bash
docker start my-mongodb
```
