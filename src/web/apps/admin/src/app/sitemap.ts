import { type MetadataRoute } from 'next';

const sitemap = (): MetadataRoute.Sitemap => {
  return [
    {
      url: 'https://sisa.cafe',
      lastModified: new Date(),
    },
    {
      url: 'https://sisa.cafe/about',
      lastModified: new Date(),
    },
    {
      url: 'https://sisa.cafe/blog',
      lastModified: new Date(),
    },
  ];
};

export default sitemap;
