declare global {
  namespace NodeJS {
    interface ProcessEnv {
      REST_API_HOST: string;
      DEFAULT_PAGE_SIZE: number;
    }
  }
}

export {};
