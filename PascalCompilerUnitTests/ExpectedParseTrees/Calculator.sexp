(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'S'
			Letter 'i'
			Letter 'm'
			Letter 'p'
			Letter 'l'
			Letter 'e'
			Letter 'C'
			Letter 'a'
			Letter 'l'
			Letter 'c'
			Letter 'u'
			Letter 'l'
			Letter 'a'
			Letter 't'
			Letter 'o'
			Letter 'r'
		)
	)
	Semicolon ';'
	(ProgramBlock
		(Block
			(LabelDeclarationPart)
			(ConstantDefinitionPart)
			(TypeDefinitionPart)
			(VariableDeclarationPart
				Var 'var'
				(VariableDeclaration
					(IdentifierList
						(Identifier
							Letter 'n'
							Letter 'u'
							Letter 'm'
							Digit '1'
						)
						Comma ','
						(Identifier
							Letter 'n'
							Letter 'u'
							Letter 'm'
							Digit '2'
						)
						Comma ','
						(Identifier
							Letter 'r'
							Letter 'e'
							Letter 's'
							Letter 'u'
							Letter 'l'
							Letter 't'
						)
					)
					Colon ':'
					(TypeDenoter
						(TypeIdentifier
							(Identifier
								Letter 'r'
								Letter 'e'
								Letter 'a'
								Letter 'l'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier
							Letter 'o'
							Letter 'p'
							Letter 'e'
							Letter 'r'
							Letter 'a'
							Letter 't'
							Letter 'o'
							Letter 'r'
						)
					)
					Colon ':'
					(TypeDenoter
						(TypeIdentifier
							(Identifier
								Letter 'c'
								Letter 'h'
								Letter 'a'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
			)
			(ProcedureAndFunctionDeclarationPart)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter first number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter first number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter first number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '1'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'u'
																								Letter 'm'
																								Digit '1'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '1'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '1'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '1'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
															Digit '1'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
															Digit '1'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ReadParameterList
											OpenParen '('
											(Ambiguous VariableAccess
												(VariableAccess
													(EntireVariable
														(VariableIdentifier
															(Identifier
																Letter 'n'
																Letter 'u'
																Letter 'm'
																Digit '1'
															)
														)
													)
												)
												(VariableAccess
													(ComponentVariable
														(FieldDesignator
															(FieldDesignatorIdentifier
																(Identifier
																	Letter 'n'
																	Letter 'u'
																	Letter 'm'
																	Digit '1'
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous ReadlnParameterList
											(ReadlnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '1'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(ReadlnParameterList
												OpenParen '('
												(Ambiguous VariableAccess
													(VariableAccess
														(EntireVariable
															(VariableIdentifier
																(Identifier
																	Letter 'n'
																	Letter 'u'
																	Letter 'm'
																	Digit '1'
																)
															)
														)
													)
													(VariableAccess
														(ComponentVariable
															(FieldDesignator
																(FieldDesignatorIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '1'
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '1'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '1'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '1'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous WritelnParameterList
											(WritelnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '1'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(WritelnParameterList
												OpenParen '('
												(WriteParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '1'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'u'
																								Letter 'm'
																								Digit '1'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '1'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '1'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter operator (+, -, *, /): ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter operator (+, -, *, /): ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter operator (+, -, *, /): ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'o'
																				Letter 'p'
																				Letter 'e'
																				Letter 'r'
																				Letter 'a'
																				Letter 't'
																				Letter 'o'
																				Letter 'r'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'o'
																							Letter 'p'
																							Letter 'e'
																							Letter 'r'
																							Letter 'a'
																							Letter 't'
																							Letter 'o'
																							Letter 'r'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'o'
																								Letter 'p'
																								Letter 'e'
																								Letter 'r'
																								Letter 'a'
																								Letter 't'
																								Letter 'o'
																								Letter 'r'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'o'
																					Letter 'p'
																					Letter 'e'
																					Letter 'r'
																					Letter 'a'
																					Letter 't'
																					Letter 'o'
																					Letter 'r'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'o'
																					Letter 'p'
																					Letter 'e'
																					Letter 'r'
																					Letter 'a'
																					Letter 't'
																					Letter 'o'
																					Letter 'r'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'o'
																		Letter 'p'
																		Letter 'e'
																		Letter 'r'
																		Letter 'a'
																		Letter 't'
																		Letter 'o'
																		Letter 'r'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'o'
																			Letter 'p'
																			Letter 'e'
																			Letter 'r'
																			Letter 'a'
																			Letter 't'
																			Letter 'o'
																			Letter 'r'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'o'
															Letter 'p'
															Letter 'e'
															Letter 'r'
															Letter 'a'
															Letter 't'
															Letter 'o'
															Letter 'r'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'o'
															Letter 'p'
															Letter 'e'
															Letter 'r'
															Letter 'a'
															Letter 't'
															Letter 'o'
															Letter 'r'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ReadParameterList
											OpenParen '('
											(Ambiguous VariableAccess
												(VariableAccess
													(EntireVariable
														(VariableIdentifier
															(Identifier
																Letter 'o'
																Letter 'p'
																Letter 'e'
																Letter 'r'
																Letter 'a'
																Letter 't'
																Letter 'o'
																Letter 'r'
															)
														)
													)
												)
												(VariableAccess
													(ComponentVariable
														(FieldDesignator
															(FieldDesignatorIdentifier
																(Identifier
																	Letter 'o'
																	Letter 'p'
																	Letter 'e'
																	Letter 'r'
																	Letter 'a'
																	Letter 't'
																	Letter 'o'
																	Letter 'r'
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous ReadlnParameterList
											(ReadlnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'o'
																		Letter 'p'
																		Letter 'e'
																		Letter 'r'
																		Letter 'a'
																		Letter 't'
																		Letter 'o'
																		Letter 'r'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'o'
																			Letter 'p'
																			Letter 'e'
																			Letter 'r'
																			Letter 'a'
																			Letter 't'
																			Letter 'o'
																			Letter 'r'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(ReadlnParameterList
												OpenParen '('
												(Ambiguous VariableAccess
													(VariableAccess
														(EntireVariable
															(VariableIdentifier
																(Identifier
																	Letter 'o'
																	Letter 'p'
																	Letter 'e'
																	Letter 'r'
																	Letter 'a'
																	Letter 't'
																	Letter 'o'
																	Letter 'r'
																)
															)
														)
													)
													(VariableAccess
														(ComponentVariable
															(FieldDesignator
																(FieldDesignatorIdentifier
																	(Identifier
																		Letter 'o'
																		Letter 'p'
																		Letter 'e'
																		Letter 'r'
																		Letter 'a'
																		Letter 't'
																		Letter 'o'
																		Letter 'r'
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'o'
																			Letter 'p'
																			Letter 'e'
																			Letter 'r'
																			Letter 'a'
																			Letter 't'
																			Letter 'o'
																			Letter 'r'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'o'
																						Letter 'p'
																						Letter 'e'
																						Letter 'r'
																						Letter 'a'
																						Letter 't'
																						Letter 'o'
																						Letter 'r'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'o'
																							Letter 'p'
																							Letter 'e'
																							Letter 'r'
																							Letter 'a'
																							Letter 't'
																							Letter 'o'
																							Letter 'r'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'o'
																				Letter 'p'
																				Letter 'e'
																				Letter 'r'
																				Letter 'a'
																				Letter 't'
																				Letter 'o'
																				Letter 'r'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'o'
																				Letter 'p'
																				Letter 'e'
																				Letter 'r'
																				Letter 'a'
																				Letter 't'
																				Letter 'o'
																				Letter 'r'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous WritelnParameterList
											(WritelnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'o'
																		Letter 'p'
																		Letter 'e'
																		Letter 'r'
																		Letter 'a'
																		Letter 't'
																		Letter 'o'
																		Letter 'r'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'o'
																			Letter 'p'
																			Letter 'e'
																			Letter 'r'
																			Letter 'a'
																			Letter 't'
																			Letter 'o'
																			Letter 'r'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(WritelnParameterList
												OpenParen '('
												(WriteParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'o'
																				Letter 'p'
																				Letter 'e'
																				Letter 'r'
																				Letter 'a'
																				Letter 't'
																				Letter 'o'
																				Letter 'r'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'o'
																							Letter 'p'
																							Letter 'e'
																							Letter 'r'
																							Letter 'a'
																							Letter 't'
																							Letter 'o'
																							Letter 'r'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'o'
																								Letter 'p'
																								Letter 'e'
																								Letter 'r'
																								Letter 'a'
																								Letter 't'
																								Letter 'o'
																								Letter 'r'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'o'
																					Letter 'p'
																					Letter 'e'
																					Letter 'r'
																					Letter 'a'
																					Letter 't'
																					Letter 'o'
																					Letter 'r'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'o'
																					Letter 'p'
																					Letter 'e'
																					Letter 'r'
																					Letter 'a'
																					Letter 't'
																					Letter 'o'
																					Letter 'r'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter second number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter second number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter second number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '2'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'u'
																								Letter 'm'
																								Digit '2'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '2'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '2'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '2'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '2'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
															Digit '2'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
															Digit '2'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ReadParameterList
											OpenParen '('
											(Ambiguous VariableAccess
												(VariableAccess
													(EntireVariable
														(VariableIdentifier
															(Identifier
																Letter 'n'
																Letter 'u'
																Letter 'm'
																Digit '2'
															)
														)
													)
												)
												(VariableAccess
													(ComponentVariable
														(FieldDesignator
															(FieldDesignatorIdentifier
																(Identifier
																	Letter 'n'
																	Letter 'u'
																	Letter 'm'
																	Digit '2'
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous ReadlnParameterList
											(ReadlnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '2'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '2'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(ReadlnParameterList
												OpenParen '('
												(Ambiguous VariableAccess
													(VariableAccess
														(EntireVariable
															(VariableIdentifier
																(Identifier
																	Letter 'n'
																	Letter 'u'
																	Letter 'm'
																	Digit '2'
																)
															)
														)
													)
													(VariableAccess
														(ComponentVariable
															(FieldDesignator
																(FieldDesignatorIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '2'
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '2'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '2'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '2'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous WritelnParameterList
											(WritelnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																		Digit '2'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																			Digit '2'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(WritelnParameterList
												OpenParen '('
												(WriteParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																				Digit '2'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'u'
																								Letter 'm'
																								Digit '2'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '2'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																					Digit '2'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(ConditionalStatement
									(CaseStatement
										Case 'case'
										(CaseIndex
											(Expression
												(SimpleExpression
													(Term
														(Ambiguous Factor
															(Factor
																(BoundIdentifier
																	(Identifier
																		Letter 'o'
																		Letter 'p'
																		Letter 'e'
																		Letter 'r'
																		Letter 'a'
																		Letter 't'
																		Letter 'o'
																		Letter 'r'
																	)
																)
															)
															(Factor
																(Ambiguous VariableAccess
																	(VariableAccess
																		(EntireVariable
																			(VariableIdentifier
																				(Identifier
																					Letter 'o'
																					Letter 'p'
																					Letter 'e'
																					Letter 'r'
																					Letter 'a'
																					Letter 't'
																					Letter 'o'
																					Letter 'r'
																				)
																			)
																		)
																	)
																	(VariableAccess
																		(ComponentVariable
																			(FieldDesignator
																				(FieldDesignatorIdentifier
																					(Identifier
																						Letter 'o'
																						Letter 'p'
																						Letter 'e'
																						Letter 'r'
																						Letter 'a'
																						Letter 't'
																						Letter 'o'
																						Letter 'r'
																					)
																				)
																			)
																		)
																	)
																)
															)
															(Factor
																(UnsignedConstant
																	(ConstantIdentifier
																		(Identifier
																			Letter 'o'
																			Letter 'p'
																			Letter 'e'
																			Letter 'r'
																			Letter 'a'
																			Letter 't'
																			Letter 'o'
																			Letter 'r'
																		)
																	)
																)
															)
															(Factor
																(FunctionDesignator
																	(FunctionIdentifier
																		(Identifier
																			Letter 'o'
																			Letter 'p'
																			Letter 'e'
																			Letter 'r'
																			Letter 'a'
																			Letter 't'
																			Letter 'o'
																			Letter 'r'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Of 'of'
										(CaseListElement
											(CaseConstantList
												(CaseConstant
													(Constant
														CharacterString ''+''
													)
												)
											)
											Colon ':'
											(Statement
												(SimpleStatement
													(Ambiguous AssignmentStatement
														(AssignmentStatement
															(Ambiguous VariableAccess
																(VariableAccess
																	(EntireVariable
																		(VariableIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
																(VariableAccess
																	(ComponentVariable
																		(FieldDesignator
																			(FieldDesignatorIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 's'
																					Letter 'u'
																					Letter 'l'
																					Letter 't'
																				)
																			)
																		)
																	)
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																	)
																	(AddingOperator
																		Plus '+'
																	)
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														(AssignmentStatement
															(FunctionIdentifier
																(Identifier
																	Letter 'r'
																	Letter 'e'
																	Letter 's'
																	Letter 'u'
																	Letter 'l'
																	Letter 't'
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																	)
																	(AddingOperator
																		Plus '+'
																	)
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										(CaseListElement
											(CaseConstantList
												(CaseConstant
													(Constant
														CharacterString ''-''
													)
												)
											)
											Colon ':'
											(Statement
												(SimpleStatement
													(Ambiguous AssignmentStatement
														(AssignmentStatement
															(Ambiguous VariableAccess
																(VariableAccess
																	(EntireVariable
																		(VariableIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
																(VariableAccess
																	(ComponentVariable
																		(FieldDesignator
																			(FieldDesignatorIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 's'
																					Letter 'u'
																					Letter 'l'
																					Letter 't'
																				)
																			)
																		)
																	)
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																	)
																	(AddingOperator
																		Minus '-'
																	)
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														(AssignmentStatement
															(FunctionIdentifier
																(Identifier
																	Letter 'r'
																	Letter 'e'
																	Letter 's'
																	Letter 'u'
																	Letter 'l'
																	Letter 't'
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																	)
																	(AddingOperator
																		Minus '-'
																	)
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										(CaseListElement
											(CaseConstantList
												(CaseConstant
													(Constant
														CharacterString ''*''
													)
												)
											)
											Colon ':'
											(Statement
												(SimpleStatement
													(Ambiguous AssignmentStatement
														(AssignmentStatement
															(Ambiguous VariableAccess
																(VariableAccess
																	(EntireVariable
																		(VariableIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
																(VariableAccess
																	(ComponentVariable
																		(FieldDesignator
																			(FieldDesignatorIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 's'
																					Letter 'u'
																					Letter 'l'
																					Letter 't'
																				)
																			)
																		)
																	)
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																		(MultiplyingOperator
																			Asterisk '*'
																		)
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														(AssignmentStatement
															(FunctionIdentifier
																(Identifier
																	Letter 'r'
																	Letter 'e'
																	Letter 's'
																	Letter 'u'
																	Letter 'l'
																	Letter 't'
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																		(MultiplyingOperator
																			Asterisk '*'
																		)
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										(CaseListElement
											(CaseConstantList
												(CaseConstant
													(Constant
														CharacterString ''/''
													)
												)
											)
											Colon ':'
											(Statement
												(SimpleStatement
													(Ambiguous AssignmentStatement
														(AssignmentStatement
															(Ambiguous VariableAccess
																(VariableAccess
																	(EntireVariable
																		(VariableIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
																(VariableAccess
																	(ComponentVariable
																		(FieldDesignator
																			(FieldDesignatorIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 's'
																					Letter 'u'
																					Letter 'l'
																					Letter 't'
																				)
																			)
																		)
																	)
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																		(MultiplyingOperator
																			ForwardSlash '/'
																		)
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														(AssignmentStatement
															(FunctionIdentifier
																(Identifier
																	Letter 'r'
																	Letter 'e'
																	Letter 's'
																	Letter 'u'
																	Letter 'l'
																	Letter 't'
																)
															)
															Walrus ':='
															(Expression
																(SimpleExpression
																	(Term
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '1'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																		)
																		(MultiplyingOperator
																			ForwardSlash '/'
																		)
																		(Ambiguous Factor
																			(Factor
																				(BoundIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																						Digit '2'
																					)
																				)
																			)
																			(Factor
																				(Ambiguous VariableAccess
																					(VariableAccess
																						(EntireVariable
																							(VariableIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'u'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																					)
																					(VariableAccess
																						(ComponentVariable
																							(FieldDesignator
																								(FieldDesignatorIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'u'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			(Factor
																				(UnsignedConstant
																					(ConstantIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(Factor
																				(FunctionDesignator
																					(FunctionIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										End 'end'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Result: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'r'
																							Letter 'e'
																							Letter 's'
																							Letter 'u'
																							Letter 'l'
																							Letter 't'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'r'
																								Letter 'e'
																								Letter 's'
																								Letter 'u'
																								Letter 'l'
																								Letter 't'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 's'
																					Letter 'u'
																					Letter 'l'
																					Letter 't'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'r'
																					Letter 'e'
																					Letter 's'
																					Letter 'u'
																					Letter 'l'
																					Letter 't'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'r'
																		Letter 'e'
																		Letter 's'
																		Letter 'u'
																		Letter 'l'
																		Letter 't'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'r'
																			Letter 'e'
																			Letter 's'
																			Letter 'u'
																			Letter 'l'
																			Letter 't'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'r'
															Letter 'e'
															Letter 's'
															Letter 'u'
															Letter 'l'
															Letter 't'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'r'
															Letter 'e'
															Letter 's'
															Letter 'u'
															Letter 'l'
															Letter 't'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Result: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'r'
																			Letter 'e'
																			Letter 's'
																			Letter 'u'
																			Letter 'l'
																			Letter 't'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 's'
																						Letter 'u'
																						Letter 'l'
																						Letter 't'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'r'
																							Letter 'e'
																							Letter 's'
																							Letter 'u'
																							Letter 'l'
																							Letter 't'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Result: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'r'
																			Letter 'e'
																			Letter 's'
																			Letter 'u'
																			Letter 'l'
																			Letter 't'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'r'
																						Letter 'e'
																						Letter 's'
																						Letter 'u'
																						Letter 'l'
																						Letter 't'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'r'
																							Letter 'e'
																							Letter 's'
																							Letter 'u'
																							Letter 'l'
																							Letter 't'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'r'
																				Letter 'e'
																				Letter 's'
																				Letter 'u'
																				Letter 'l'
																				Letter 't'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(EmptyStatement)
							)
						)
					)
					End 'end'
				)
			)
		)
	)
	Dot '.'
)