import { CircleAlert, Files, Gauge, LucideIcon } from "lucide-react";

type MenuItemType = {
  title: string;
  url: string;
  external?: string;
  icon?: LucideIcon;
  items?: MenuItemType[];
};
type MenuType = MenuItemType[];

export const mainMenu: MenuType = [
  {
    title: "Home",
    url: "/",
    icon: Gauge,
  },
  {
    title: "Pages",
    url: "/pages",
    icon: Files,
    items: [
      {
        title: "Product Page",
        url: "/pages/products",
      },
      {
        title: "Coming Soon",
        url: "/pages/feature",
      },
    ],
  },
  {
    title: "Error",
    url: "/404",
    icon: CircleAlert,
  },
];
