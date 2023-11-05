interface Category {
  id: string;
  name: string;
  slug: string;
  picture?: string;
  description?: string;
  parentId?: string[];
  parent?: Partial<Category>;
  creator: Partial<User>;
  createdAt: Date;
  updater?: Partial<User>;
  updatedAt?: Date;
}

interface User {
  id: string;
  fullName: string;
  userName: string;
}
