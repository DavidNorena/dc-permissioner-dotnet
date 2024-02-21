## Development Environment Setup

For contributors and developers working on the Permissioneer library, setting up a local development environment is streamlined using Docker and user-secrets for secure configuration management.

### MSSQL Server with Docker

A `docker-compose.yaml` file is included at the root of the project to facilitate the setup of an MSSQL server instance for development and testing purposes. To spin up the MSSQL server, ensure you have Docker installed and running on your machine, then execute the following command:

```bash
docker compose up mssql -d
```

This command starts a Docker container running MSSQL Server in detached mode, using the configuration specified in `docker-compose.yml`. The MSSQL server's password is set using the `MSSQL_SA_PASSWORD` environment variable, which should be defined in a `.env` file at the root of the project.

### Configuration File

Before running the command, make sure to create a `.env` file based on the provided `.env.example` file. This file includes necessary environment variables for the Docker container, including the database password.

### Setting Up User Secrets

For local development, particularly when working with the TestAPI project (a test API for validating the library's integration and configuration), it's important to securely manage database credentials using user secrets. Set the `PermissioneerDbPassword` user secret in the `src/TestAPI` project by running the following command from the terminal:

```bash
dotnet user-secrets set "PermissioneerDbPassword" "YourStrongPasswordHere"
```

Replace "YourStrongPasswordHere" with a secure password of your choice. This command adds the specified secret to your local user secrets store, which is used by the application to securely access the database without hardcoding sensitive information in your source code.

### Contributing

We welcome contributions! Please feel free to improve the library, fix bugs, or suggest enhancements. For more details on how to contribute, please refer to this CONTRIBUTING.md file.
