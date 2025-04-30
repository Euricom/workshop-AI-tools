# API Wrapper Implementation Guidelines

- **File Structure**
  - Place API wrappers in `src/api/[resource]/` folders
  - Each resource should have:
    ```
    src/api/[resource]/
    ├── types.ts        # Type definitions
    ├── [resource].ts   # API implementation
    └── index.ts        # Public exports
    ```

- **Type Definitions**
  - Define DTOs using OpenAPI schema types:
    ```typescript
    // ✅ DO: Import from generated schema
    import { components } from "../schema";
    export type ProductDTO = components["schemas"]["product"];
    
    // ❌ DON'T: Define raw types manually
    export type ProductDTO = {
      id: string;
      name: string;
      // ...
    };
    ```
  - Map DTOs to domain models with proper type transformations:
    ```typescript
    // ✅ DO: Transform dates and complex types
    export type Product = Omit<ProductDTO, "createdAt" | "updatedAt"> & {
      createdAt: Date;
      updatedAt: Date;
    };
    ```

- **API Implementation**
  - Use the ApiInstance type for consistent HTTP client usage:
    ```typescript
    // ✅ DO: Accept ApiInstance as first parameter
    export const getProduct = async (
      api: ApiInstance,
      id: string
    ): Promise<Product> => {
      // implementation
    };
    
    // ❌ DON'T: Create new HTTP clients or use fetch directly
    export const getProduct = async (id: string) => {
      const response = await fetch(`/api/products/${id}`);
      // ...
    };
    ```
  
  - Implement data mapping functions:
    ```typescript
    // ✅ DO: Create mapper functions for type transformations
    const productMapper = (product: ProductDTO): Product => ({
      ...product,
      createdAt: new Date(product.createdAt),
      updatedAt: new Date(product.updatedAt),
    });
    
    // ❌ DON'T: Transform data inline
    const data = await api.get<ProductDTO>(`/api/products/${id}`);
    return {
      ...data,
      createdAt: new Date(data.createdAt),
      // ...
    };
    ```

- **Query Parameters**
  - Define parameter interfaces for endpoints with query params:
    ```typescript
    // ✅ DO: Create typed parameter interfaces
    export interface GetProductsParams {
      page?: number;
      pageSize?: number;
      sortBy?: string;
      filter?: string;
    }
    
    // ❌ DON'T: Use any or Record<string, unknown>
    export const getProducts = (params: any) => {
      // ...
    };
    ```

- **Error Handling**
  - Let the ApiInstance handle errors centrally
  - Don't implement error handling in individual API wrappers unless specific handling is needed

- **Best Practices**
  - Keep API wrappers focused on data fetching and transformation
  - Use TypeScript for full type safety
  - Follow RESTful naming conventions
  - Keep URLs consistent with backend API structure
  - Use proper HTTP methods (GET, POST, PUT, DELETE)
  - Implement proper pagination handling where needed