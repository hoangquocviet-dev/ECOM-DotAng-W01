import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CmsService } from '../../../core/services/cms.service';
import { IBlog } from '../../../models/blog.model';

@Component({
  selector: 'app-blogs',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './blogs.component.html',
  styleUrls: ['./blogs.component.scss']
})
export class BlogsComponent implements OnInit {
  blogs: IBlog[] = [];

  constructor(private cmsService: CmsService) {}

  ngOnInit(): void {
    this.cmsService.getBlogs().subscribe({
      next: (data) => {
        this.blogs = data;
      }
    });
  }
}
