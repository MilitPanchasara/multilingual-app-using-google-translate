import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { LanguageService } from './language.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'demo-project';

  content = null;
  languageList = [];
  hidePage = false;

  constructor(public languageService: LanguageService) {
    this.languageService.languageChangedSubject.subscribe(x => {
      this.hidePage = x;
    });
    this.languageList = languageService.languagesList;
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

  changeLanguage(code) {
    localStorage.setItem("language", code);
    this.languageService.getTranslations(code);
  }
}
