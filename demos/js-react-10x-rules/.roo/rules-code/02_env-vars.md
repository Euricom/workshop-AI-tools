# Guidelines how to add environment variable

## Environment Variables

This is how we add environment variables to the project:

  1. Add to `.env.example`:
      ```bash
      VITE_NEW_VARIABLE=value_example
      ```
  2. Add to `env.ts`:
      ```typescript
      // For client-side variables
      const envSchema = z.object({
        VITE_NEW_VARIABLE: z.string(),
      })
      ```

Environment variables are always prefixed by `VITE_`

### References

#file:src/env.ts
#file:.env.example

