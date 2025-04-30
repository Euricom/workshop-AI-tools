# React Query patterns for data fetching, loading states, and error handling in pages

- **Use React Query for Data Fetching**
  - Always use `useQuery` hook for fetching data in page components
  - Define descriptive query keys for effective caching and invalidation
  - Set default values for data to avoid null checks
  - Use type parameters to ensure type safety
  ```typescript
  // ✅ DO: Proper React Query usage in page component
  const api = useApiInstance();
  const { data: products = [], error, isLoading } = useQuery<Product[]>({
    queryKey: ["products"],
    queryFn: () => getProducts(api),
  });
  
  // ✅ DO: Add parameters to query keys when filtering/sorting
  const { data: filteredProducts = [] } = useQuery<Product[]>({
    queryKey: ["products", { category, sortBy }],
    queryFn: () => getProducts(api, { filter: category, sortBy }),
    enabled: !!category, // Only run query when category is available
  });
  ```

- **Handle Loading States Properly**
  - Always handle the initial loading state
  - Use skeleton components for better user experience
  - Add loading indicators for refetching states
  ```typescript
  // ✅ DO: Handle loading state
  const { data: products = [], isLoading, error } = useQuery<Product[]>({
    queryKey: ["products"],
    queryFn: () => getProducts(api),
  });

  if (isLoading) {
    return (
      <Card className="p-6">
        <TableSkeleton columns={6} rows={5} />
      </Card>
    );
  }

  // ❌ DON'T: Ignore loading states
  const { data: products = [] } = useQuery<Product[]>({
    queryKey: ["products"],
    queryFn: () => getProducts(api),
  });
  // This will render an empty table before data loads
  ```

- **Implement Consistent Error Handling**
  - Use toast notifications for error messages
  - Implement error boundaries for critical components
  - Configure retry logic for transient errors
  ```typescript
  // ✅ DO: Handle fetch errors
  useEffect(() => {
    if (error) {
      toast.error(`Something went wrong: ${error.message}`);
    }
  }, [error]);

  // More comprehensive error handling
  if (error) {
    return (
      <Card className="p-6">
        <div className="text-red-600">
          <h3>Error loading products</h3>
          <p>{error.message}</p>
          <Button onClick={() => refetch()}>Retry</Button>
        </div>
      </Card>
    );
  }
  ```

- **Structure Components for Data Display**
  - Separate data fetching from presentation logic
  - Use conditional rendering for different states
  - Implement proper type checking for API data
  ```typescript
  // ✅ DO: Proper component structure
  export default function ProductsPage() {
    // 1. Data fetching
    const api = useApiInstance();
    const { 
      data: products = [], 
      isLoading, 
      error 
    } = useQuery<Product[]>({
      queryKey: ["products"],
      queryFn: () => getProducts(api),
    });

    // 2. Error handling
    useEffect(() => {
      if (error) {
        toast.error(`Something went wrong: ${error.message}`);
      }
    }, [error]);

    // 3. Conditional rendering based on state
    if (isLoading) {
      return <ProductsTableSkeleton />;
    }

    // 4. Component rendering with data
    return (
      <>
        <PageHeader>
          <PageHeaderHeading>Product Page</PageHeaderHeading>
        </PageHeader>
        <ProductsTable products={products} />
      </>
    );
  }
  ```

- **Data Flow Diagram**
  ```mermaid
  flowchart TD
    A[Page Component] --> B[useQuery Hook]
    B --> C[queryFn]
    C --> D[External Data]
    B --> F[isLoading State]
    B --> H[data State]
    F --> I[Loading UI Components]
    H --> K[Data Display Components]