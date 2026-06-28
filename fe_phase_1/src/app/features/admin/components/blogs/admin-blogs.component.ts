import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminBlogService, AdminBlog } from '../../../../core/services/admin-blog.service';

@Component({
  selector: 'app-admin-blogs',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-blogs.component.html',
  styleUrls: ['./admin-blogs.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminBlogsComponent {
  private blogService = inject(AdminBlogService);
  blogs$ = this.blogService.getBlogs();

  trackById(index: number, item: AdminBlog): number {
    return item.id;
  }
}
