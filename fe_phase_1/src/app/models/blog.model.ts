export interface IBlog {
  id: string;
  title: string;
  slug: string;
  thumbnail: string;
  summary: string;
  content?: string;
  author: string;
  publishedDate: Date;
  tags?: string[];
}
