import React, { useEffect, useState } from 'react';
import { useAgendamento } from './AgendamentoContext';
import { FormAgendamento } from './FormAgendamento';
import { ListaAgendamentos } from './ListaAgendamentos';
import './index.css';

function App() {
  const { carregarAgendamentos, erro, mensagemSucesso, limparMensagem, agendamentos } = useAgendamento();
  const [abaMostrada, setAbaMostrada] = useState<'formulario' | 'lista'>('formulario');

  useEffect(() => {
    carregarAgendamentos();
  }, []);

  useEffect(() => {
    if (mensagemSucesso) {
      const timer = setTimeout(() => {
        limparMensagem();
        setAbaMostrada('lista');
      }, 2000);
      return () => clearTimeout(timer);
    }
  }, [mensagemSucesso, limparMensagem]);

  const totalAgendamentos = agendamentos.length;

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      {/* Header */}
      <header className="bg-gradient-to-r from-blue-600 to-indigo-600 text-white p-6 shadow-lg">
        <div className="max-w-6xl mx-auto">
          <h1 className="text-3xl font-bold">💉 Sistema de Agendamento de Vacinação</h1>
          <p className="text-blue-100 mt-2">COVID-19</p>
        </div>
      </header>

      {/* Notificações */}
      {erro && (
        <div className="max-w-6xl mx-auto mt-4 p-4 bg-red-100 border border-red-400 text-red-700 rounded">
          {erro}
        </div>
      )}

      {mensagemSucesso && (
        <div className="max-w-6xl mx-auto mt-4 p-4 bg-green-100 border border-green-400 text-green-700 rounded">
          {mensagemSucesso}
        </div>
      )}

      {/* Conteúdo Principal */}
      <div className="max-w-6xl mx-auto p-6">
        {/* Abas */}
        <div className="flex gap-4 mb-6">
          <button
            onClick={() => setAbaMostrada('formulario')}
            className={`px-6 py-2 rounded-lg font-semibold transition ${
              abaMostrada === 'formulario'
                ? 'bg-blue-600 text-white shadow-lg'
                : 'bg-white text-gray-700 hover:bg-gray-100'
            }`}
          >
            Novo Agendamento
          </button>
          <button
            onClick={() => setAbaMostrada('lista')}
            className={`px-6 py-2 rounded-lg font-semibold transition flex items-center gap-2 ${
              abaMostrada === 'lista'
                ? 'bg-blue-600 text-white shadow-lg'
                : 'bg-white text-gray-700 hover:bg-gray-100'
            }`}
          >
            Consultar Agendamentos
            {totalAgendamentos > 0 && (
              <span className="bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center text-sm">
                {totalAgendamentos}
              </span>
            )}
          </button>
        </div>

        {/* Conteúdo das Abas */}
        {abaMostrada === 'formulario' ? (
          <FormAgendamento onSucesso={() => setAbaMostrada('lista')} />
        ) : (
          <ListaAgendamentos />
        )}
      </div>

      {/* Footer */}
      <footer className="bg-gray-800 text-white text-center p-4 mt-12">
        <p>© 2024 Sistema de Agendamento de Vacinação - Desafio Técnico</p>
      </footer>
    </div>
  );
}

export default App;
