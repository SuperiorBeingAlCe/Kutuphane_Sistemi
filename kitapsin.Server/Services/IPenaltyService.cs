using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Ceza işlemleri için servis arayüzü.
    /// </summary>
    public interface IPenaltyService
    {
        /// <summary>
        /// Yeni bir ceza ekler.
        /// </summary>
        /// <param name="dto">Ceza oluşturma DTO'su.</param>
        /// <returns>Eklenen cezanın yanıt DTO'su.</returns>
        Task<DtoPenaltyResponse> AddAsync(DtoPenaltyCreate dto);

        /// <summary>
        /// Belirtilen cezayı öder ve kaldırır.
        /// </summary>
        /// <param name="id">Ceza kimliği.</param>
        /// <returns>İşlem başarılıysa true, aksi halde false.</returns>
        Task<bool> PayAndRemoveAsync(int id);

        /// <summary>
        /// Kimliğe göre ceza getirir.
        /// </summary>
        /// <param name="id">Ceza kimliği.</param>
        /// <returns>Ceza yanıt DTO'su veya null.</returns>
        Task<DtoPenaltyResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Tüm cezaları getirir.
        /// </summary>
        /// <returns>Ceza yanıt DTO'larının listesi.</returns>
        Task<IEnumerable<DtoPenaltyResponse>> GetAllAsync();
    }
}
