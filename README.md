# Teste Api Docker

Este projeto contém uma API ASP.NET Core com um front-end Vue já embutido no mesmo contêiner.

## Executando dois clientes

O `docker-compose.yml` foi configurado para subir duas cópias da aplicação, cada uma com seu próprio banco PostgreSQL. Para iniciar tudo, execute:

```bash
docker compose up --build
```

Após a inicialização, acesse:

- [http://localhost:8080](http://localhost:8080) - cliente 1
- [http://localhost:8081](http://localhost:8081) - cliente 2

Cada cliente utiliza um banco separado (`client1db` e `client2db`).
