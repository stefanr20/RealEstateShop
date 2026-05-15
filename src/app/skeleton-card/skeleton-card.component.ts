import { Component } from '@angular/core';

@Component({
  selector: 'app-skeleton-card',
  standalone: true,
  template: `
    <div class="skeleton-card">
      <div class="skeleton-img shimmer"></div>
      <div class="skeleton-body">
        <div class="skeleton-line wide shimmer"></div>
        <div class="skeleton-line medium shimmer"></div>
        <div class="skeleton-stats shimmer"></div>
        <div class="skeleton-btn shimmer"></div>
      </div>
    </div>
  `,
  styles: [`
    .skeleton-card {
      background: #fff;
      border-radius: 16px;
      overflow: hidden;
      border: 1px solid #f0f0f0;
    }

    .skeleton-img {
      width: 100%;
      height: 220px;
      background: #f0f0f0;
    }

    .skeleton-body {
      padding: 20px;
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .skeleton-line {
      height: 14px;
      border-radius: 6px;
      background: #f0f0f0;
    }

    .skeleton-line.wide { width: 80%; }
    .skeleton-line.medium { width: 50%; }

    .skeleton-stats {
      height: 40px;
      border-radius: 6px;
      background: #f0f0f0;
    }

    .skeleton-btn {
      height: 38px;
      border-radius: 8px;
      background: #f0f0f0;
      margin-top: 4px;
    }

    .shimmer {
      background: linear-gradient(90deg, #f0f0f0 25%, #e8e8e8 50%, #f0f0f0 75%);
      background-size: 200% 100%;
      animation: shimmer 1.4s infinite;
    }

    @keyframes shimmer {
      0% { background-position: 200% 0; }
      100% { background-position: -200% 0; }
    }
  `]
})
export class SkeletonCardComponent {}
