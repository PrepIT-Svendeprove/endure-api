CREATE USER authentik WITH PASSWORD 'authentik';
CREATE DATABASE authentik OWNER authentik;

GRANT ALL PRIVILEGES ON DATABASE authentik TO authentik;