import axios from 'axios';

const API_BASE_URL = 'http://localhost:5190/api';

export interface Agendamento {
  id: number;
  nomePaciente: string;
  dataNascimento: string;
  dataAgendamento: string;
  horario: string;
  status: string;
  conclusao: string;
}

export interface CriarAgendamentoRequest {
  nome: string;
  dataNascimento: string;
  dataAgendamento: string;
  horario: string;
}

export const api = {
  async listarAgendamentos(): Promise<Agendamento[]> {
    const response = await axios.get(`${API_BASE_URL}/agendamentos`);
    return response.data;
  },

  async criarAgendamento(dados: CriarAgendamentoRequest): Promise<{ message: string }> {
    const response = await axios.post(`${API_BASE_URL}/agendamentos`, dados);
    return response.data;
  },

  async atualizarStatus(id: number, status: string, conclusao: string): Promise<void> {
    await axios.put(`${API_BASE_URL}/agendamentos/${id}/status`, {
      status,
      conclusao
    });
  }
};
