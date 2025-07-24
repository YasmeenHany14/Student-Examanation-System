export interface CreateSubjectModel {
  name: string;
  code: string;
}

export interface GetSubjectModel {
  id: number;
  name: string;
  code: string | null;
  hasConfiguration?: boolean | null;
}

export interface UpdateSubjectModel {
  name: string;
  code: string;
}
