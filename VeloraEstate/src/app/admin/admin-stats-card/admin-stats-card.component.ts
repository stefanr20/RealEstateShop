import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'card-c',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-stats-card.component.html',
  styleUrls: ['./admin-stats-card.component.css']
})
export class AdminStatsCardComponent {
  @Input() value: number = 0;
  @Input() label: string = '';
}