# Project Structure

## Main Structure

- App is in root folder

```tree
├── public/            # Public assets
├── src/               # Application source code
|   ├── api/           # API types and wrappers
│   ├── components/    # Other React components
│   ├── components/ui  # Shadcn components
│   ├── context/       # contexts components
│   ├── config/        # Config data
│   ├── hook/          # Custom hooks
│   ├── lib/           # Utility functions
│   ├── pages/         # pages/features components
│   ├── App.tsx        # Application entry point
│   ├── index.css      # Main css and tailwind configuration
│   ├── main.tsx       # Main rendering file
│   └── Router.tsx     # Routes component
├── index.html         # HTML entry point
├── tsconfig.json      # TypeScript configuration
└── vite.config.ts     # Vite configuration
```

## File Naming and Organization

- Use PascalCase for components (e.g. `src/components/Button.tsx`) 
- Shadcn components are in `src/components/ui`
- All other components are in `src/components/`
- Colocate files in the folder where they're used unless they can be used across the app
- If a component can be used in many places, place it in the `src/components` folder

## New Pages

- Add new pages in the `src/pages` directory.
- Organize each page in its own folder: `src/pages/PAGE_NAME/`.
- Place the main page component inside the folder as `Page.tsx`.
- Include any page-specific components within the same folder for better organization.

## Utility Functions

- Place reusable utility functions in the `src/lib/` directory to maintain a clean and organized structure.
- Leverage `lodash` for common operations involving arrays, objects, and strings to simplify code and improve readability.
- Import only the specific `lodash` functions you need to reduce the bundle size. For example:

```ts
import groupBy from "lodash/groupBy";
```
- Write utility functions with clear and concise documentation to ensure ease of use and maintainability.
- Avoid duplicating logic by centralizing shared functionality in the `lib/` folder.
- Test all utility functions thoroughly to ensure reliability across the application.

