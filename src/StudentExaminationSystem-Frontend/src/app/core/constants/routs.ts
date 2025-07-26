export class routes {
  public static readonly baseUrl = 'https://localhost:5001/api';
  public static readonly authLogin = '/account/login';
  public static readonly authRegisterStudent = '/student';
  public static readonly authLogout = '/account/logout';
  public static readonly authRefresh = '/account/refresh-token';
  public static readonly subjectsDropdown = '/subject/all';
  public static readonly studentSubjectDropdown = '/subject/all';
  public static readonly subjects = '/subject';
  public static readonly dashboard = '/home/dashboard';
  public static readonly adminHomePage = routes.dashboard;
  public static readonly studentHomePage = '/home/exams';
  public static readonly subjectConfig = 'subject/${id}/exam-config'
  public static readonly difficultyProfile = '/difficulty-profile';
  public static readonly difficultyProfileDropdown =  routes.difficultyProfile + '/all';
  public static readonly students = '/student';
  public static readonly exam = '/exam';

  public static SetSubjectConfig(id: number | string) {
    return `/subject/${id}/exam-config`;
  }

}
