

# Ejecutar
## migraciones de base de datos
Primero crear la base de datos en SQL server y especificar el connection string en appsettings.

Luego ejecutar migración
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```


# IA
Instalar ollama

## Comandos 

```
# Ver todos los modelos descargados en local
ollama list

# Descargar un modelo (última versión disponible)
ollama pull llama3


# Descargar una versión específica
ollama pull llama3:8b
ollama pull llama3:70b


# Ejecutar modelo en modo interactivo (chat en terminal)
ollama run llama3


# Endpoint local que expone Ollama

POST http://localhost:11434/api/generate

{
  "model": "llama3",
  "prompt": "Explica qué es una API",
  "temperature": 0.7
}


# Borrar un modelo para liberar espacio
ollama rm llama3


# Re-descargar modelo (actualiza si hay nueva versión)
ollama pull llama3
```

