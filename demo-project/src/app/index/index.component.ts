import { Component, OnInit } from '@angular/core';
import { LanguageService } from '../language.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  content = null;

  constructor(public languageService: LanguageService) {
    this.content = localStorage.getItem("data");
    if (this.content == undefined || this.content == null) {
      if(localStorage.getItem("language") == undefined || localStorage.getItem("language") == null) {
        localStorage.setItem("language",'en');
      }
      this.languageService.getTranslations(localStorage.getItem("language"));
    }
    else {
      this.content = JSON.parse(localStorage.getItem("data"));
    }
  }

  ngOnInit(): void {
  } 
}
