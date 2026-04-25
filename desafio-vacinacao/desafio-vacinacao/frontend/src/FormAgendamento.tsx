import React, { useState } from 'react';
import { api } from './api';
import { useAgendamento } from './AgendamentoContext';

interface FormAgendamentoProps {
  onSucesso: () => void;
}

export const FormAgendamento: React.FC<FormAgendamentoProps> = ({ onSucesso }) => {
  const { definirErro, definirSucesso, carregarAgendamentos } = useAgendamento();
  const [carregando, setCarregando] = useState(false);
  const [formData, setFormData] = useState({
    nome: '',
    dataNascimento: '',
    dataAgendamento: '',
    horario: ''
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.nome || !formData.dataNascimento || !formData.dataAgendamento || !formData.horario) {
      definirErro('Todos os campos são obrigatórios');
      return;
    }

    setCarregando(true);
    definirErro(null);

    try {
      await api.criarAgendamento(formData);
      definirSucesso('Agendamento realizado com sucesso!');
      setFormData({
        nome: '',
        dataNascimento: '',
        dataAgendamento: '',
        horario: ''
      });
      await carregarAgendamentos();
      onSucesso();
    } catch (err: any) {
      const mensagem = err.response?.data?.message || 'Erro ao criar agendamento';
      definirErro(mensagem);
    } finally {
      setCarregando(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Novo Agendamento</h2>
      
      <div className="mb-4">
        <label className="block text-gray-700 font-semibold mb-2">Nome Completo</label>
        <input
          type="text"
          name="nome"
          value={formData.nome}
          onChange={handleChange}
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:border-blue-500"
          placeholder="Digite seu nome"
        />
      </div>

      <div className="mb-4">
        <label className="block text-gray-700 font-semibold mb-2">Data de Nascimento</label>
        <input
          type="date"
          name="dataNascimento"
          value={formData.dataNascimento}
          onChange={handleChange}
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:border-blue-500"
        />
      </div>

      <div className="mb-4">
        <label className="block text-gray-700 font-semibold mb-2">Data do Agendamento</label>
        <input
          type="date"
          name="dataAgendamento"
          value={formData.dataAgendamento}
          onChange={handleChange}
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:border-blue-500"
        />
      </div>

      <div className="mb-4">
        <label className="block text-gray-700 font-semibold mb-2">Horário</label>
        <input
          type="time"
          name="horario"
          value={formData.horario}
          onChange={handleChange}
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:border-blue-500"
        />
      </div>

      <button
        type="submit"
        disabled={carregando}
        className="w-full bg-blue-600 text-white font-semibold py-2 rounded-lg hover:bg-blue-700 disabled:bg-gray-400 transition"
      >
        {carregando ? 'Agendando...' : 'Agendar Vacinação'}
      </button>
    </form>
  );
};
