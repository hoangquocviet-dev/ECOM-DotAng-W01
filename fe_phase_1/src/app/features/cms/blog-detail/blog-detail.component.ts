import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CmsService } from '../../../core/services/cms.service';
import { IBlog } from '../../../models/blog.model';

@Component({
  selector: 'app-blog-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.scss']
})
export class BlogDetailComponent implements OnInit {
  blog: IBlog | undefined;
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private cmsService: CmsService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.fetchBlog(slug);
      }
    });
  }

  fetchBlog(slug: string): void {
    this.isLoading = true;
    this.cmsService.getBlogBySlug(slug).subscribe({
      next: (data) => {
        this.blog = data;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }
}
