import "@typespec/http";
import "@typespec/versioning";

using TypeSpec.Http;


@service({
  title: "WebShop",
  description: "API for WebShop",
})
namespace WebShop {

  //
  // Models
  //
  model User {
    id: string;
    username: string;
    email: string;
    createdAt: string;
  }

  model UserRegistration {
    username: string;
    email: string;
    password: string;
  }

  model UserLogin {
    username: string;
    password: string;
  }

  model Product {
    id: string;
    name: string;
    description: string;
    price: float32;
    available: boolean;
    category: string;
  }

  model ProductUpdate {
    // Optional properties for update operations
    name?: string;
    description?: string;
    price?: float32;
    available?: boolean;
    category?: string;
  }

  model BasketItem {
    productId: string;
    quantity: int32;
  }

  model Basket {
    userId: string;
    items: BasketItem[];
    total: float32;
  }

  //
  // Services
  //

  @route("/users")
  @tag("Users")
  interface UserService {
    // Create a new user
    @post
    op create(user: UserRegistration): User;

    // Retrieve a user by ID
    @route("/{userId}")
    @get
    op getUser(userId: string): User;
  }

  @route("/auth")
  @tag("Auth")
  interface AuthService {
    // Log in a user and return a JWT token
    @route("/login")
    @post
    op login(credentials: UserLogin): { token: string };
  }

  @route("/products")
  @tag("Products")
  interface ProductService {
    // List products with optional filtering and pagination
    @get
    op listProducts(
      @query category?: string,
      @query page?: int32,
      @query pageSize?: int32
    ): Product[];

    // Retrieve details of a specific product
    @route("/{productId}")
    @get
    op getProduct(productId: string): Product;

    // Update a product (e.g., by an admin)
    @route("/{productId}")
    @put
    op updateProduct(
      productId: string,
      product: ProductUpdate
    ): Product;

    // Delete a product (e.g., by an admin)
    @route("/{productId}")
    @delete
    op deleteProduct(productId: string): void;
  }

  @route("/basket")
  @tag("Basket")
  interface BasketService {
    // Get the basket for the authenticated user
    @get
    op getBasket(): Basket;

    // Add an item to the basket
    @post
    op addItem(item: BasketItem): Basket;

    // Remove an item from the basket
    @route("/{itemId}")
    @delete
    op removeItem(itemId: string): Basket;
  }
}